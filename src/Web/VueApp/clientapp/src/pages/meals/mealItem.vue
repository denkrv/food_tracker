<template>
    <div>
        {{item.Name}}
    </div>
</template>

<script>

    export default {
        name: 'meal_item',
        props: {
            item: { type: Object }
        },
        data: () => {
            return {}
        },
        created() {
            console.log(this);
        },
        methods: {
            save() {               
            },
            cancel() {
                this.$router.go(-1);
            }
        }
    }
</script>


<style scoped>
</style>
