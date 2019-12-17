import { cloneDeep } from 'lodash';
//import Vue from "vue";


export const RESET_PROFILE = 'resetProfile';

const initialState = {
    profile : {
        weight: 90,
        estimates: {
            calories: 2200,
            protein : 248,
            fat: 49,
            carbohydrates: 293
        }
    }
}

export const state =  cloneDeep(initialState);

const mutations = {
    setProfile(state, profile) {
        state.profile = profile;
    }
}

const actions = {
    
}

const getters = {
    profile(state) {
        return state.profile;
    },
    estimates(state) {
        return state.profile.estimates;
    }
};


export default {
    state,
    mutations,
    actions,
    getters

}