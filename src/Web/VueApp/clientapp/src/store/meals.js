import { FETCH_MEALS, MEAL_DELETE } from './actions.type'
import { MealsService } from '@/common/api.service'

export const SET_LOADING = "setLoading";
export const SET_MEALS = "setMeals";
export const SET_MEALS_COUNT = "setMealsCount";

const initialState = {
    meals: [],
    mealsCount: 0,
    isLoading : true
}

const state = { ...initialState };


const getters = {
    mealsCount(state) {
        return state.mealsCount;
    },
    meals(state) {
        return state.meals;
    },
    isLoading(state) {
        return state.isLoading;
    }
};


const mutations = {
    [SET_LOADING](state, value) {
        state.isLoading = value;
    },
    [SET_MEALS](state, meals) {
        state.meals = meals;
    },
    [SET_MEALS_COUNT](state, count) {
        state.mealsCount = count;
    },
    resetMeals(state) {
        state.meals = [];
        state.mealsCount = 0;
    }
  
}

const actions = {
    [FETCH_MEALS]({ commit }, params) {
        commit(SET_LOADING, true);
        commit('resetMeals');
        return MealsService.query(params)
            .then(({ data }) => {
                commit(SET_MEALS, data.items);
                commit(SET_MEALS_COUNT, data.totalCount);
                commit(SET_LOADING, false);
            });
    },
    [MEAL_DELETE]({ commit, dispatch }, meal) {
        if (meal.id)
            MealsService.delete(meal.id).then(() => {
                commit('resetMeals');
                return dispatch(FETCH_MEALS, {});
            });
    }
}


export default {
    state,
    getters,
    actions,
    mutations
}