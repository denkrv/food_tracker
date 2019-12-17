<template>
    <div>
        <h3>Edit meal</h3>
        <b-table small :items="mealItems" :fields="fields" foot-clone v-show="mealItems.length>0">
            <template v-slot:cell(amount)="data">
                <input type="number" placeholder="amount" v-bind:value="data.item.amount" v-on:input="updateAmount(data.item, $event.target.value)"/>
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
                <b-button size="sm" class="mr-1" @click="removeItem(row.item)">
                    remove
                </b-button>
            </template>
            <template v-slot:foot(name)="data">
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

        <div v-if="!isAdding" class="row">
            <div class="col">
                <b-button size="sm" class="mr-1" @click="startAdding">
                    Add food
                </b-button>
            </div>
        </div>
        <div class="row" v-else>
            <div class="col">
                <FoodLookup v-if="isFoodSelect" v-on:selected="handleSelected"></FoodLookup>
                <b-link v-else @click.prevent="isFoodSelect = true" class="mr-3">{{selectedItem.name}}</b-link>

                <input type="number" placeholder="amount" v-model.number="selectedItem.amount" v-if="!isFoodSelect && selectedItem.amount" class="mr-3" />

                <button class="btn btn-outline-secondary" @click="addItem" v-if="!isFoodSelect && selectedItem.amount" v-bind:disabled=" !(selectedItem.amount > 0)">add</button>
            </div>

        </div>
        <div class="row mt-3">
            <div class="col">
                <b-button size="m" class="mr-3" @click="save" v-show="mealItems.length > 0">
                    Save
                </b-button>
                <b-button size="m" @click="cancel">
                    Cancel
                </b-button>
            </div>
        </div>
    </div>
</template>

<script>

    /*eslint-disable */
    import { FETCH_MEAL, MEAL_ADD_ITEM, MEAL_REMOVE_ITEM, SAVE_MEAL } from '@/store/actions.type';
    import { RESET_MEAL, SET_MEAL_ITEM_AMOUNT } from '@/store/meal';
    import MealItem from './mealItem';
    import FoodLookup from '@/components/FoodLookup';
    import store from '@/store'
    import { mapGetters } from 'vuex';

    /*eslint-enable */
    export default {
        name: 'meal_edit',
        /*eslint-disable */
        // eslint-disable-next-line vue/no-unused-components
        components: { MealItem, FoodLookup },
        /*eslint-enable */
        data: () => {
            return {
                fields: ['name', 'amount', 'protein', 'carbohydrates', 'fat', 'calories', { key: 'actions', label: '' }],
                isNew: false,
                selectedItem: {},
                isAdding: false,
                isFoodSelect: false,
                fromRoute : null
            }
        },
        computed: {
            ...mapGetters(["meal", "mealItems"]),
            totals() {
                return this.mealItems.reduce((prev, current) => {
                    Object.keys(prev).map(k => prev[k] += current.facts[k]);
                    return prev;
                },
                    { protein: 0, carbohydrates: 0, fat: 0, calories: 0 });
            }
        },

        beforeRouteUpdate(to, from, next) {            
            store.commit(RESET_MEAL);
            return next();
        },

         async beforeRouteEnter(to, from, next) {
            store.commit(RESET_MEAL);
            if (to.params.id !== undefined) {
                await store.dispatch(FETCH_MEAL, to.params.id);
            }
            return next(vm => {
                vm.fromRoute = from;
            });
        },

        methods: {
            startAdding() {
                this.isAdding = true;
                this.isFoodSelect = true;
            },
            handleSelected(item) {
                console.log('selected');
                this.selectedItem = {
                    foodId: item.id,
                    facts: item.facts,
                    name: item.name,
                    amount: 100
                };
                this.isFoodSelect = false;

            },
            addItem() {
                if (this.selectedItem && this.selectedItem.foodId > 0 && this.selectedItem.amount >= 0) {
                    this.$store.dispatch(MEAL_ADD_ITEM, this.selectedItem);
                    console.log('addItem');
                    this.selectedItem = {};
                    this.isAdding = false;
                }
            },
            removeItem(item) {
                this.$store.dispatch(MEAL_REMOVE_ITEM, item);
            },
            updateAmount(item, amount) {
                let value = parseInt(amount);                
                if (!isNaN(value)) {
                    console.log(value);
                    this.$store.commit(SET_MEAL_ITEM_AMOUNT, { foodId: item.foodId, amount: value });
                }                
            },

            save() {
                this.$store.dispatch(SAVE_MEAL);                
                this.handleBack();
            },
            cancel() {
                this.handleBack();
            },

            handleBack() {
                if (!this.fromRoute) {
                    this.$router.back();
                } else
                    this.$router.push({ name: 'meals' });
            }
        }
    }
</script>


<style scoped>
</style>
