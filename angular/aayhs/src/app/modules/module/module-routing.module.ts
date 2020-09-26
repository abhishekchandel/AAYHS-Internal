import { NgModule } from '@angular/core';
import { LayoutComponent } from '../perspective/components/layout/layout.component';
import { Routes, RouterModule } from '@angular/router';
import { YearlyMaintenanceComponent } from './components/yearly-maintenance/yearly-maintenance.component';

const routes: Routes = [
  {
    path: "",
    component: LayoutComponent,
    children: [
      {
        path: "yearly-maintenance",
        component: YearlyMaintenanceComponent,
        data: {
          title: "Yearly Maintenance"
        }
      },
     
     
      
    ],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ModuleRoutingModule { }
