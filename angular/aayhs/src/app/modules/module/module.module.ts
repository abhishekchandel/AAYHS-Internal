import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { YearlyMaintenanceComponent } from './module/yearly-maintenance/yearly-maintenance.component';
import { SharedModule } from '../../shared/shared.module';
import { ModuleRoutingModule } from './module-routing.module';



@NgModule({
  declarations: [YearlyMaintenanceComponent],
  imports: [
    CommonModule,
    SharedModule,
    ModuleRoutingModule
  ]
})
export class ModuleModule { }
