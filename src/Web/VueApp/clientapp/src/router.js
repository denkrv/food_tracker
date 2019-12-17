import Vue from 'vue'
import Router from 'vue-router'


Vue.use(Router);

const routes = [
    { path: '/products', name: 'products', component: () => import(/* webpackChunkName: "views" */'@/pages/products/list.vue') },
    { path: '/products/:id', name: 'product-edit', component: () => import(/* webpackChunkName: "views" */'@/pages/products/edit.vue') },
    { path: '/products/add', name: 'product-add', component: () => import(/* webpackChunkName: "views" */'@/pages/products/edit.vue') },

    { path: '/meals', name: 'meals', component: () => import(/* webpackChunkName: "views" */'@/pages/meals/list.vue') },
    { path: '/meals/:id(\\d+)', name: 'meals-edit', component: () => import(/* webpackChunkName: "views" */'@/pages/meals/edit.vue') },
    { path: '/meals/add', name: 'meals-add', component: () => import(/* webpackChunkName: "views" */'@/pages/meals/edit.vue') },

    { path: '/recipes', name: 'recipes', component: () => import(/* webpackChunkName: "views" */'@/pages/recipes/list.vue') },
    {
        path: '/',
        redirect: '/meals'
    }
    //{ path: '/bar', component: Bar }
]

export default new Router({
    mode: 'history',
    routes
});