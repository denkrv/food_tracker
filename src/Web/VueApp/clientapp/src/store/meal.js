import { cloneDeep } from 'lodash';
import Vue from "vue";
import { FETCH_MEAL, MEAL_ADD_ITEM, MEAL_REMOVE_ITEM, SAVE_MEAL } from './actions.type'
import { MealsService } from '@/common/api.service'

export const RESET_MEAL = 'resetMeal';
export const SET_MEAL = 'setMealState';
export const ADD_MEAL_ITEM = 'addMealItem';
export const REMOVE_MEAL_ITEM = 'rmMealItem';
export const SET_MEAL_ITEM_AMOUNT = 'setMealItemAmount';


const initialState = {
    meal: {
        consumed: null,
        items: []
    }
}

export const state =  cloneDeep(initialState);

const mutations = {
    [SET_MEAL](state, meal) {
        state.meal = meal;
    },
    [ADD_MEAL_ITEM](state, item) {
        let existing = state.meal.items.find((i) => i.foodId === item.foodId);
        if (existing) {
            existing.amount += item.amount;
        }else
            state.meal.items.push(item);
    },
    //required refactoring to use foodId
    [REMOVE_MEAL_ITEM](state, item) {
        state.meal.items = state.meal.items.filter(i => i !== item);
    },
    [RESET_MEAL](state) {
        for (let f in initialState) {
            Vue.set(state, f, cloneDeep(initialState[f]));
        }
    },
    [SET_MEAL_ITEM_AMOUNT](state, {foodId, amount }) {
        let item = state.meal.items.find((i) => i.foodId === foodId);
        if (item) {
            item.amount = amount;
            //Vue.set(item, 'amount', amount);
        }
    }

}

const actions = {
    [FETCH_MEAL]({ commit }, id) {
        return MealsService.get(id)
            .then(({ data }) => {
                console.log(data);
                commit(SET_MEAL, data);
            });
    },
    [MEAL_ADD_ITEM]({ commit }, item) {
        commit(ADD_MEAL_ITEM, item);
    },
    [MEAL_REMOVE_ITEM]({ commit }, item) {
        commit(REMOVE_MEAL_ITEM, item);
    },
    [SAVE_MEAL]({ commit, state }) {
        if (state.meal.items.length) {
            MealsService.save(state.meal).then(() => commit(RESET_MEAL));
            
        }
    }
}

const getters = {
    meal(state) {
        return state.meal;
    },
    mealItems(state) {
        return state.meal.items;
    }
};


export default {
    state,
    mutations,
    actions,
    getters

}