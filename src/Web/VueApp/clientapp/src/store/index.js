import Vue from "vue";
import Vuex from "vuex";
import meals from "./meals";
import meal from "./meal";
import profile from "./profile";

Vue.use(Vuex);

export default new Vuex.Store({
    modules: {
        meals,
        meal,
        profile
    }
});