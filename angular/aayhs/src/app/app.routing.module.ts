import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './core/guards/auth-guard'


const routes: Routes = [
  {
      path: "",
      loadChildren: () => import(`./modules/auth/auth.module`).then(m => m.AuthModule)
  },
  {
    path: "auth",
    loadChildren: () => import(`./modules/perspective/perspective.module`).then(m => m.PerspectiveModule),
    // canActivate:[AuthGuard]
},
];
@NgModule({
  imports:
      [
          RouterModule.forRoot(routes)
      ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
