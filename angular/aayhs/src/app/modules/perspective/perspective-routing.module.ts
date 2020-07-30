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
    children: [
      {
        path: "sponsor",
        component: SponsorComponent,
        data: {
          title: "Sponser"
        }
      },
      {
        path: "horse",
        component: HorseComponent,
        data: {
          title: "Horse"
        }
      },
      {
        path: "exhibitor",
        component: ExhibitorComponent,
        data: {
          title: "Exhibitor"
        }
      },
      {
        path: "class",
        component: ClassComponent,
        data: {
          title: "Class"
        }
      },
      {
        path: "group",
        component: GroupComponent,
        data: {
          title: "Group"
        }
      },
    ],
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PerspectiveRoutingModule { }
