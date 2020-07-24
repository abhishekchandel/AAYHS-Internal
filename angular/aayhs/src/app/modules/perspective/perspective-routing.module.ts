import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SponsorComponent } from './components/sponsor/sponsor.component';
import { HorseComponent } from './components/horse/horse/horse.component';
import { ExhibitorComponent } from './components/exhibitor/exhibitor/exhibitor.component';
import { ClassComponent } from './components/class/class/class.component';
import { GroupComponent } from './components/group/group/group.component';
import { LayoutComponent } from './components/layout/layout/layout.component';


const routes: Routes = [
  {
  path: "",
    component: LayoutComponent,
    children:   [
  {
    path: "sponsor",
    component: SponsorComponent
  },
  {
    path: "horse",
    component: HorseComponent
  },
  {
    path: "exhibitor",
    component: ExhibitorComponent
  },
  {
    path: "class",
    component: ClassComponent
  },
  {
    path: "group",
    component: GroupComponent
  },
],
}
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PerspectiveRoutingModule { }
