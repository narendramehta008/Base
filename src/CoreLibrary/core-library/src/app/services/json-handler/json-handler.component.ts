import { Component } from '@angular/core';
import { UtilsService } from '@app/shared/services/utils.service';

@Component({
  selector: 'app-json-handler',
  templateUrl: './json-handler.component.html',
  styleUrl: './json-handler.component.scss',
})
export class JsonHandlerComponent {
  data = {
    'simple key': 'simple value',
    numbers: 1234567,
    'simple list': ['value1', 22222, 'value3'],
    'special value': undefined,
    owner: null,
    'simple obect': {
      'simple key': 'simple value',
      numbers: 1234567,
      'simple list': ['value1', 22222, 'value3'],
      'simple obect': {
        key1: 'value1',
        key2: 22222,
        key3: 'value3',
      },
    },
  };

  constructor(private utils: UtilsService) {}

  loadData() {
    // this.utils
    //   .getRequest('https://official-joke-api.appspot.com/jokes/random/250')
    //   .subscribe((resp) => {
    //     console.log(resp);
    //     if (typeof resp == 'string') this.data = resp;
    //   });
  }
}
