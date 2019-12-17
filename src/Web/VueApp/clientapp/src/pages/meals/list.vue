<template>
    <div>
        <MealsHeader v-model="date"></MealsHeader>

        <b-table small :items="meals" :fields="fields" foot-clone>
            <template v-slot:cell(intakeTime)="data">
                {{formatTime(data.item.intakeTime)}}
            </template>
            <template v-slot:cell(protein)="data">
                {{data.item.facts.protein}}
            </template>
            <template v-slot:cell(carbohydrates)="data">
                {{data.item.facts.carbohydrates}}
            </template>
            <template v-slot:cell(fat)="data">
                {{data.item.facts.fat}}
            </template>
            <template v-slot:cell(calories)="data">
                {{data.item.facts.calories}}
            </template>
            <template v-slot:cell(actions)="row">
                <b-button size="sm" class="mr-1" @click="navigateEdit(row.item.id)">
                    Edit
                </b-button>
                <b-button size="sm" class="mr-1" @click="deleteMeal(row.item)">
                    Delete
                </b-button>
            </template>
            <template v-slot:foot(intakeTime)="data">
                <i>Total</i>
            </template>
            <template v-slot:foot(protein)="data">
                <i>{{totals.protein}}</i>
            </template>
            <template v-slot:foot(fat)="data">
                <i>{{totals.fat}}</i>
            </template>
            <template v-slot:foot(carbohydrates)="data">
                <i>{{totals.carbohydrates}}</i>
            </template>
            <template v-slot:foot(calories)="data">
                <i>{{totals.calories}}</i>
            </template>
            <template v-slot:foot()="data">
            </template>
        </b-table>
        <div>
            <b-button size="sm" @click="navigateAdd()" class="mr-1">
                Add new
            </b-button>
        </div>
        <div class="row" v-show="meals.length>0">
            <div class="col-sm-6">
                <apexchart class="chart" type="donut" :series="caloriesSeq" :options="compositionChartOptions"></apexchart>
            </div>
            <div class="col-sm-6">
                <apexchart class="chart" :options="estChartOptions" :series="estimatesSeq"></apexchart>
            </div>
        </div>
    </div>
</template>

<script>
    import { mapGetters } from 'vuex';
    import * as moment from 'moment';
    import { at, debounce } from 'lodash';

    import { FETCH_MEALS, MEAL_DELETE } from '@/store/actions.type';    
    import { genCaloriesSeq, calcCalories } from '@/common/util';
    
    import MealsHeader from './mealsHeader';    
    import compositionChartOptions from './compositionChartOptions';
    import estimationsChartOptions from './estimationsChartOptions';
    

    const fields = ["protein", "carbohydrates", "fat"];
    export default {
        name: 'meals_list',
        components: { MealsHeader},
        data() {
            return {
                fields: [{ key: 'intakeTime', label: 'Time' }, 'protein', 'carbohydrates', 'fat', 'calories', { key: 'actions', label: 'Actions' }],
                currentPage: 1,
                date: new Date(),
                compositionChartOptions: compositionChartOptions,
                estChartOptions: estimationsChartOptions
            }
        },
        computed: {
            ...mapGetters(["isLoading", "mealsCount", "meals", "estimates"]),
            totals() {
                let facts = this.meals.reduce((prev, current) => {
                    fields.map(k => prev[k] += current.facts[k]);
                    return prev;
                }, { protein: 0, carbohydrates: 0, fat: 0 });
                facts.calories = calcCalories(facts);
                return facts;

            },
            caloriesSeq() {
                return genCaloriesSeq(this.totals);
            },
            
            estimatesSeq() {                
                let consumed = at(this.totals, fields);
                let tmp = at(this.estimates, fields).map((v, i) => v - consumed[i]);
                let remaining = tmp.map((v) => Math.max(v, 0));
                let over = tmp.map((v) => -Math.min(v, 0));
                return [{
                    name: 'consumed',
                    data: consumed
                }, {
                    name: 'remaining',
                    data: remaining
                }, {
                    name: 'overeaten',
                    data: over
                }]
            }
        },
        mounted() {
            this.fetch();
        },
        created() {
            this.debouncedFetch = debounce(() => this.fetch(), 500, { leading : true});
        },
        methods: {
            fetch() {
                let start = moment(this.date).startOf('date');
                let end = moment(this.date).endOf('date');
                this.$store.dispatch(FETCH_MEALS, {
                    start: start.toJSON(),
                    end: end.toJSON(),
                });
            },
            formatTime(time) {                
                return moment(time).format('L LT');
            },
            navigateEdit(id) {
                this.$router.push({ name: 'meals-edit', params: { id } });
            },
            navigateAdd() {
                this.$router.push({ name: 'meals-add' });
            },
            deleteMeal(meal) {
                this.$store.dispatch(MEAL_DELETE, meal);
            }
            
        },
        watch: {
            date() {
                this.debouncedFetch();                
            }
        }

    }</script>


<style scoped>
    .chart {
        max-width: 400px;
    }
</style>
