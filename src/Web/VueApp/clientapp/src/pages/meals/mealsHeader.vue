<template>
    <h3 class="mb-4">
        <a href="#" aria-label="Previous" @click.stop.prevent="changeDate(prevDate)" class="mr-1">
            <span aria-hidden="true">&laquo;</span>
        </a>
        
        <date-picker valueType="date" :editable="false" :clearable="false" v-model="val" input-class="" @input="changeDate">
            <template v-slot:input>
                <span>{{formatedDate}}</span>
            </template>
            <template v-slot:icon-calendar><span></span>             
            </template>
            
        </date-picker>
        <a href="#" aria-label="Next" @click.stop.prevent="changeDate(nextDate)">
            <span aria-hidden="true">&raquo;</span>
        </a>
    </h3>
    
</template>

<script>
    import * as moment from 'moment';
    import DatePicker from 'vue2-datepicker';
    import 'vue2-datepicker/index.css';
 

    export default {
        name: 'meals_header',
        components: {
            DatePicker 
        },
        props: {
            value: { type: Date }
        },   
        data() {
            return { val: this.value }
        },
        computed: {
            formatedDate() {
                return moment(this.val).format('LL');
            },
            nextDate() {
                return moment(this.val).add(1, 'd').toDate();
            },
            prevDate() {
                return moment(this.val).subtract(1,'d').toDate()
            }
        },
        methods: {
            changeDate(v) {                               
                this.val = v;
                this.$emit('input', this.val);
            },            
        }
    }
</script>


<style lang="scss">
    .mx-datepicker {
        display: inline;
        margin:0;
        padding:0;
    }
    .mx-datepicker>.mx-input-wrapper{
        display: inline;
        margin:0;
        padding:0;
    }
    .mx-icon-calendar{
        position:relative;
    }
</style>
