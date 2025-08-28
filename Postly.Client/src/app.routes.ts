import { Routes } from '@angular/router';
import Layout from './pages/layout/layout';
import Home from './pages/home/home';
import Login from './pages/login/login';
import Register from './pages/register/register';
import Profile from './pages/profile/profile';

export const routes: Routes = [
    {
        path: '',
        component: Layout,
        children: [
            {
                path: '',
                component: Home,
            },
            {
                path: 'login',
                component: Login,
            },
            {
                path: 'register',
                component: Register,
            },
            {
                path: 'profile',
                component: Profile,
            }
        ]
    }
];
