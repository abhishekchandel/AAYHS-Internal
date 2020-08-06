import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PerspectiveRoutingModule } from './perspective-routing.module';
import { ClassComponent } from './components/class/class.component';
import { ExhibitorComponent } from './components/exhibitor/exhibitor/exhibitor.component';
import { GroupComponent } from './components/group/group/group.component';
import { HorseComponent } from './components/horse/horse/horse.component';
import { SharedModule } from '../../shared/shared.module';
import { LayoutComponent } from './components/layout/layout/layout.component';
import { SponsorComponent } from './components/sponsor/sponsor.component';


@NgModule({
  declarations: [ClassComponent, ExhibitorComponent, GroupComponent,LayoutComponent, HorseComponent,SponsorComponent],
  imports: [
    CommonModule,
    PerspectiveRoutingModule,
    SharedModule,
  ]
})
export class PerspectiveModule { }
