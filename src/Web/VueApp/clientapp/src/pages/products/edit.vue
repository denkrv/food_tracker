<template>
    <div>
        <form>
            <div class="form-group row">
                <label for="name-edit" class="col-sm-2 col-form-label">Name</label>
                <div class="col-sm">
                    <input type="text" class="form-control" id="name-edit" v-model="food.name" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm">
                    <div class="form-group row">
                        <label for="prot-edit" class="col-sm-4 col-form-label">Protein</label>
                        <div class="col-sm-4">
                            <input type="number" class="form-control" id="prot-edit" v-model.number="food.facts.protein" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="carb-edit" class="col-sm-4 col-form-label">Carbohydrates</label>
                        <div class="col-sm-4">
                            <input type="number" class="form-control" id="carb-edit" v-model.number="food.facts.carbohydrates" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="fat-edit" class="col-sm-4 col-form-label">Fat</label>
                        <div class="col-sm-4">
                            <input type="number" class="form-control" id="fat-edit" v-model.number="food.facts.fat" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="calories-edit" class="col-sm-4 col-form-label">Calories</label>
                        <div class="col-sm-4">
                            <input type="number" class="form-control" id="calories-edit" v-model.number="food.facts.calories" />
                        </div>
                    </div>
                </div>
                <div class="col">
                    <apexchart class="chart" type="donut" :series="caloriesSeq" :options="chartOptions"></apexchart>
                </div>
            </div>

            <div class="form-group row">
                <div class="col">
                    <b-button variant="primary" size="md" @click="save()" class="mr-1 ">
                        Save
                    </b-button>
                    <b-button size="md" @click="cancel()" class="mr-1">
                        Back
                    </b-button>
                </div>
            </div>
        </form>
    </div>
</template>

<script>
    import { genCaloriesSeq } from '@/common/util';

    export default {
        name: 'product_edit',

        data: () => {
            return {
                chartOptions: {
                    colors: ['#87CEEB', '#FA8072', '#F0E68C'],
                    dataLabels: {
                        style: {
                            colors: ['#333', '#333', '#333']
                        },
                        dropShadow: {
                            enabled: false
                        }
                    },
                    labels: ['protein', 'carbohydrates', 'fat'],
                    tooltip: {
                        theme : 'light',
                         style: {
                            colors: ['#333', '#333', '#333']
                        }
                    }

                },                
                food: {
                    id: null,
                    name: '',
                    facts: {
                        protein: null,
                        carbohydrates: null,
                        fat: null,
                        calories: null
                    }
                },
                isNew: false,
                fromRoute: null
            }
        },
        beforeRouteEnter(to, from, next) {
            next(vm => {
                vm.fromRoute = from;
            })
        },
        created() {
            console.log(this);
            this.isNew = this.$route.name === 'product-add';
            if (!this.isNew) {
                this.axios
                    .get('products/' + this.$route.params.id)
                    .then((resp) => {
                        this.food = resp.data;
                    });
            }
        },
        watch: {
            'food.facts.protein': 'recalc',
            'food.facts.carbohydrates': 'recalc',
            'food.facts.fat': 'recalc',
        },
        computed: {
            caloriesSeq() {
                return genCaloriesSeq(this.food.facts);
            }
        },
        methods: {
            recalc() {
                this.food.facts.calories = this.caloriesSeq.reduce((p, c) => p + c, 0);
            },
            save() {
                if (this.isNew) {
                    this.axios
                        .post('products/', this.food)
                        .then(() => {
                            this.handleBack();
                        });
                } else {
                    this.axios
                        .put('products/' + this.$route.params.id, this.food)
                        .then(() => {
                            this.handleBack();
                        });
                }
            },
            cancel() {
                this.handleBack();
            },

            handleBack() {
                if (!this.fromRoute) {
                    this.$router.back();
                } else
                    this.$router.push({ name: 'products' });
            }

        }
    }
</script>


<style scoped>
    .chart {
        max-width: 350px;
    }
    
 
</style>
