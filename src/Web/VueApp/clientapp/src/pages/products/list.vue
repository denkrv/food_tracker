<template>
    <div>
        <div>
            <b-button size="sm" @click="navigateAdd()" class="mr-1">
                Add new product
            </b-button>
        </div>
        <b-table small :items="items" :fields="fields" id="products-table">
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
                <b-button size="sm" @click="navigateEdit(row.item.id)" class="mr-1">
                    Edit
                </b-button>
                <b-button size="sm" @click="deleteItem(row.item.id)" class="mr-1">
                    Delete
                </b-button>
            </template>
        </b-table>
        <b-pagination v-model="currentPage"
                      :total-rows="totalCount"
                      :per-page="pageSize" aria-controls="products-table" :hide-ellipsis="true" :limit ="10"></b-pagination>
    </div>
</template>

<script>
    import { debounce } from 'lodash';
    export default {
        name: 'products_list',
        data: () => {
            return {
                fields: ['name', 'protein', 'carbohydrates', 'fat', 'calories', { key: 'actions', label: 'Actions' }],
                items: [],
                totalCount: 0,
                currentPage: 1,
                pageSize: 10
            }
        },
        created() {
            this.debouncedFetch = debounce(() => this.fetch(), 500, { leading : true});
        },
        mounted() {
            this.fetch();
        },
        methods: {
            fetch() {
                this.axios
                    .get('products', {
                        params: {
                            page: this.currentPage,
                            count: this.pageSize
                        }
                    })
                    .then((resp) => {
                        this.items = resp.data.items;
                        this.totalCount = resp.data.totalCount;
                        this.currentPage = resp.data.currentPage;
                        this.pageSize = resp.data.pageSize;
                    });
            },

            navigateAdd() {
                this.$router.push({ name: 'product-add' });
            },
            navigateEdit(id) {
                this.$router.push({ name: 'product-edit', params: { id } });
            },
            deleteItem(id) {
                this.$bvModal.msgBoxConfirm('Delete this product?', { okTitle: 'Yes', title: 'Confirmation required' })
                    .then((value) => {
                        console.log(value);
                        if (value) {
                            this.axios.delete('products/' + id).then(() => this.fetch());
                        }
                    });
            }            
        },
        watch: {
            currentPage() {
                this.debouncedFetch();
            }
        }
    }
</script>


<style scoped>
</style>
