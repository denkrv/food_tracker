<template>
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container">
                <!--<router-link class="navbar-brand">Tracker</router-link>-->
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                        aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse d-sm-inline-flex flex-sm-row-reverse">
                    <ul class="navbar-nav flex-grow-1">
                        <li class="nav-item">
                            <router-link class="nav-link text-dark" to="/meals">Meals</router-link>
                        </li>
                        <li class="nav-item">
                            <router-link class="nav-link text-dark" to="/products">Products</router-link>
                        </li>
                        <li class="nav-item">
                            <router-link class="nav-link text-dark" to="/recipes">Recipes</router-link>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </header>
</template>