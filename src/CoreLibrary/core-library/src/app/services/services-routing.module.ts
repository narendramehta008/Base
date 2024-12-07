import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ServicesComponent } from './services.component';
import { JsonHandlerComponent } from './json-handler/json-handler.component';
import { UtilsService } from '@app/shared/services/utils.service';
import { LearningComponent } from './learning/learning.component';
import { DataHandlerComponent } from './data-handler/data-handler.component';
import { ToolsComponent } from './tools/tools.component';

export const routes: Routes = [
  {
    path: '',
    component: ServicesComponent,
    data: {
     
      depth: 2,
      icon: 'https://i.pinimg.com/736x/63/af/5e/63af5e0a705a886c3bf316ab72264f99.jpg',
       
    },
  },
  {
    path: 'json-handler',
    component: JsonHandlerComponent,
    data: {
      title: 'JSON Handler',
      depth: 3,
      icon: 'https://i.pinimg.com/736x/1c/42/b9/1c42b9c285907ff2e3e1ccb786d1b4f7.jpg',
      text: 'JSON (JavaScript Object Notation) is a lightweight data-interchange format.'
    },
  },
  {
    path: 'learning',
    component: LearningComponent,
    data: {
      title: 'Learning',
      depth: 3,
      icon: 'https://i.pinimg.com/736x/5e/79/b8/5e79b834353ae1ebde314f6285e22408.jpg',
      text: 'Learning is the process of acquiring new understanding, knowledge, behaviors, skills, values, attitudes, and preferences.',
     
    },
  },
  {
    path: 'data-handler',
    component: DataHandlerComponent,
    data: {
      title: 'Data Manipulation',
      depth: 3,
      icon: 'https://i.pinimg.com/736x/0c/9a/89/0c9a89c556a0e8d94659222a82dcffcc.jpg',
    },
  },
  {
    path: 'tools',
    component: ToolsComponent,
    data: {
      title: 'Tools',
      depth: 3,
      icon: 'https://img.lovepik.com/element/40053/1035.png_1200.png',
    },
  },
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ServicesRoutingModule {
  /**
   *
   */
  constructor(private utils: UtilsService) {
    // utils.routes.push({});
  }
}
