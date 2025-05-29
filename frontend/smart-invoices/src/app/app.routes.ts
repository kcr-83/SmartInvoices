import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { adminGuard } from './guards/admin.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/invoices',
    pathMatch: 'full'
  },
  {
    path: 'invoices',
    loadComponent: () => import('../app/features/invoices/invoice-list/invoice-list.component').then(m => m.InvoiceListComponent),
    canActivate: [authGuard]
  },
  {
    path: 'invoices/:id',
    loadComponent: () => import('../app/features/invoices/invoice-detail/invoice-detail.component').then(m => m.InvoiceDetailComponent),
    canActivate: [authGuard]
  },
  {
    path: 'change-requests',
    loadComponent: () => import('../app/features/change-requests/change-request-list/change-request-list.component').then(m => m.ChangeRequestListComponent),
    canActivate: [authGuard]
  },
  {
    path: 'change-requests/:id',
    loadComponent: () => import('../app/features/change-requests/change-request-detail/change-request-detail.component').then(m => m.ChangeRequestDetailComponent),
    canActivate: [authGuard]
  },
  {
    path: 'refund-requests',
    loadComponent: () => import('../app/features/refund-requests/refund-request-list/refund-request-list.component').then(m => m.RefundRequestListComponent),
    canActivate: [authGuard]
  },
  {
    path: 'refund-requests/:id',
    loadComponent: () => import('../app/features/refund-requests/refund-request-detail/refund-request-detail.component').then(m => m.RefundRequestDetailComponent),
    canActivate: [authGuard]
  },
  {
    path: 'admin',
    loadChildren: () => import('../app/features/admin/admin.routes').then(m => m.adminRoutes),
    canActivate: [authGuard, adminGuard]
  },
  {
    path: 'login',
    loadComponent: () => import('../app/features/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'unauthorized',
    loadComponent: () => import('../app/features/auth/unauthorized/unauthorized.component').then(m => m.UnauthorizedComponent)
  },
  {
    path: '**',
    redirectTo: '/invoices'
  }
];
