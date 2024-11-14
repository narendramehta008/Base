import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ServicesComponent } from './services.component';
import { JsonHandlerComponent } from './json-handler/json-handler.component';
import { UtilsService } from '@app/shared/services/utils.service';
import { LearningComponent } from './learning/learning.component';
import { DataHandlerComponent } from './data-handler/data-handler.component';

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
      depth: 3,
      icon: 'https://i.pinimg.com/736x/1c/42/b9/1c42b9c285907ff2e3e1ccb786d1b4f7.jpg',
    },
  },
  {
    path: 'learning',
    component: LearningComponent,
    data: {
      depth: 3,
      icon: 'https://i.pinimg.com/736x/5e/79/b8/5e79b834353ae1ebde314f6285e22408.jpg',
    },
  },
  {
    path: 'data-handler',
    component: DataHandlerComponent,
    data: {
      depth: 3,
      icon: 'https://i.pinimg.com/736x/0c/9a/89/0c9a89c556a0e8d94659222a82dcffcc.jpg',
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
