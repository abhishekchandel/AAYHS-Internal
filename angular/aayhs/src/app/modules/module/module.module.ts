import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { YearlyMaintenanceComponent } from './components/yearly-maintenance/yearly-maintenance.component';
import { SharedModule } from '../../shared/shared.module';
import { ModuleRoutingModule } from './module-routing.module';
import { ReportsComponent } from './components/reports/reports.component';



@NgModule({
  declarations: [YearlyMaintenanceComponent, ReportsComponent],
  imports: [
    CommonModule,
    SharedModule,
    ModuleRoutingModule
  ]
})
export class ModuleModule { }
