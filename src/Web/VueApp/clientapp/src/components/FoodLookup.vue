<template>
    <div>
        <div class="input-group mb-3">
            <input type="text" class="form-control" placeholder="search food" v-model="filter" @focus="showItems"/>
            <div class="input-group-append">
                <button class="btn btn-outline-secondary" type="button">Search</button>
            </div>
        </div>
        <div class="dropdown-content" v-show="itemsShown && items.length">
            <div v-for="item in items" :key="item.id" class="dropdown-item" @mousedown="selectItem(item)" >
                {{item.name}} ({{item.facts.calories}} kcal per 100g)
            </div>
        </div>      

    </div>
</template>

<script>
    export default {
        name: 'FoodLookup',
        props: {
            msg: String
        },
        data() {
            return {
                filter: '',
                items: [],
                itemsShown: false,
                selected: {}
            };
        },
        computed: {
            filteredItems() {
                return this.items;
            }
        },
        watch: {
            filter(oldFilter, newFilter) {
                if (oldFilter !== newFilter)
                    this.lookup();
            }
        },
        methods: {
            lookup() {
                if (this.filter && this.filter.length > 0) {
                    this.axios.get('foods', { params: { Query: this.filter } })
                        .then(({ data }) => {
                            this.items = data.items;
                            console.log(data);
                        });
                } else this.items = [];

            },
            selectItem(item) {                
                this.selected = item;
                this.itemsShown = false;
                this.$emit('selected', this.selected);
                console.log(item);
                this.filter = this.selected.name;
                //this.filter = this.selected.name;
                
            },
            showItems() {
                this.itemsShown = true;
                this.filter = '';
                
            }
        }
    }
</script>


<style scoped>
    .dropdown-content {
        position: absolute;
        background-color: #fff;
        border: 1px solid #e7ecf5;
        box-shadow: 0px -8px 34px 0px rgba(0,0,0,0.05);
        overflow: auto;
        z-index: 3;
        width:100%;
    }

    .dropdown-item {
        color: black;        
        
        padding: 8px;
        text-decoration: none;
        display: block;
        cursor: pointer;
    }

        .dropdown-item:hover {
            background-color: #e7ecf5;
        }
</style>
