import { Routes } from '@angular/router';
import { AuthGuard } from './services/auth.guard';

export const routes: Routes = [
  { 
    path: 'auth', 
    loadChildren: () => import('./auth/auth.routes').then(m => m.AUTH_ROUTES)
  },
  { 
    path: 'clients', 
    loadChildren: () => import('./clients/clients.routes').then(m => m.CLIENT_ROUTES),
    canActivate: [AuthGuard]
  },
  { 
    path: 'products', 
    loadChildren: () => import('./products/products.routes').then(m => m.PRODUCT_ROUTES),
    canActivate: [AuthGuard]
  },
  { 
    path: 'todo', 
    loadChildren: () => import('./todo/todo.routes').then(m => m.TODO_ROUTES),
    canActivate: [AuthGuard]
  },
  { path: '', redirectTo: '/clients', pathMatch: 'full' },
  { path: '**', redirectTo: '/clients' }
];