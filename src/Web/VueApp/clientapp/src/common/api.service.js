import Vue from 'vue';
import axios from 'axios'
import VueAxios from 'vue-axios';
import { API_URL } from '@/common/config';

const ApiService = {
    init() {
        Vue.use(VueAxios, axios);
        Vue.axios.defaults.baseURL = API_URL;
    },

    //get(resource, params) {
    //    return Vue.axios.get(resource, params).catch(error => {
    //        throw new Error(`ApiService ${error}`);
    //    });
    //},
    //post(resource, params) {
    //    return Vue.axios.post(resource, params).catch(error => {
    //        throw new Error(`ApiService ${error}`);
    //    });
    //},


}

export const MealsService =
{
    query(params) {
        return Vue.axios.get(`meals`, { params: params });
    },
    get(id) {
        return Vue.axios.get(`meals/${id}`);
    },
    create(meal) {
        return Vue.axios.post('meals',meal);
    },
    update(meal) {
        return Vue.axios.put(`meals/${meal.id}`, meal);
    },
    save(meal) {
        return meal.id ? this.update(meal) : this.create(meal);
    },
    delete(id) {
        return Vue.axios.delete(`meals/${id}`);
    }
}

export default ApiService;