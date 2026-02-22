import { Routes } from '@angular/router';
import { User } from './user/user';
import { Registration } from './user/registration/registration';
import { Login } from './user/login/login';
import { Dashboard } from './dashboard/dashboard';

export const routes: Routes = [
    {
        path: '', component: User,
        children: [
            { path: 'signup', component: Registration },
            { path: 'signin', component: Login }
        ]
    },
    { path: 'dashboard', component: Dashboard }
];
