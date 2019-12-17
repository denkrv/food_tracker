import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import Vue from 'vue'
import router from './router'
import App from './App.vue'
import BootstrapVue from 'bootstrap-vue'
import ApiService from './common/api.service'
import store from '@/store'
import * as moment from 'moment';
import VueApexCharts from 'vue-apexcharts'

Vue.use(VueApexCharts)
Vue.component('apexchart', VueApexCharts)

moment.locale(navigator.language);

Vue.use(BootstrapVue);

Vue.config.productionTip = false

ApiService.init();

new Vue({
    router,
    store,
    render: h => h(App),
}).$mount('#app');
