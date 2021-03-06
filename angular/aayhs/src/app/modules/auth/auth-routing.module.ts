import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import{ForgotPasswordComponent} from './components/forgot-password/forgot-password.component'
import{ResetPasswordComponent} from './components/reset-password/reset-password.component'
import { AuthComponent } from './auth.component';
import{RegisterComponent} from './components/register/register.component'

const routes: Routes = [
  {
    path: "",
    component: AuthComponent,
    children: [
      {
        path: "",
        redirectTo: "login",
        pathMatch: "full"
      },
      {
        path: "login",
        component: LoginComponent
      },
      {
        path: "forgot-password",
        component: ForgotPasswordComponent
      },
      {
        path: "reset-password",
        component: ResetPasswordComponent
      },
      {
        path: "register",
        component: RegisterComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
