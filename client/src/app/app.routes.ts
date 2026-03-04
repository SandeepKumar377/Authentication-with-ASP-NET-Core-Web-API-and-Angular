import { Routes } from '@angular/router';
import { User } from './user/user';
import { Registration } from './user/registration/registration';
import { Login } from './user/login/login';
import { Dashboard } from './dashboard/dashboard';
import { authGuard } from './shared/auth-guard';
import { AdminOnly } from './authorizeDemo/admin-only/admin-only';
import { AdminOrTeacher } from './authorizeDemo/admin-or-teacher/admin-or-teacher';
import { LibraryMemberOnly } from './authorizeDemo/library-member-only/library-member-only';
import { Under10Only } from './authorizeDemo/under10-only/under10-only';
import { ApplyForMaternityLeave } from './authorizeDemo/apply-for-maternity-leave/apply-for-maternity-leave';
import { MainLayout } from './layouts/main-layout/main-layout';
import { Forbidden } from './forbidden/forbidden';

export const routes: Routes = [
    { path: '', redirectTo: 'signin', pathMatch: 'full' },
    {
        path: '', component: User,
        children: [
            { path: 'signup', component: Registration },
            { path: 'signin', component: Login }
        ]
    },
    {
        path: '', component: MainLayout, canActivate: [authGuard],
        canActivateChild: [authGuard],
        children: [
            { path: 'dashboard', component: Dashboard },
            {
                path: 'admin-only', component: AdminOnly,
                data: { claimRequired: (c: any) => c.role === 'Admin' }
            },
            {
                path: 'admin-or-teacher', component: AdminOrTeacher,
                data: { claimRequired: (c: any) => c.role === 'Admin' || c.role === 'Teacher' }
            },
            {
                path: 'library-member-only', component: LibraryMemberOnly
            },
            {
                path: 'under-10-only', component: Under10Only
            },
            {
                path: 'apply-for-maternity-leave', component: ApplyForMaternityLeave
            },
            { path: 'forbidden', component: Forbidden },
        ]
    },
];
