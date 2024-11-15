import { ElementRef, TemplateRef } from '@angular/core';

export type Func = (...param: any[]) => any;

export class Funcs {
  func?: Func;
  error?: Func;
  final?: Func;

  constructor(func?: Func, error?: Func, final?: Func) {
    this.func = func;
    this.error = error;
    this.final = final;
  }
  execute(func?: Func, value?: any) {
    func && func(value);
  }
}

export interface IRequestParams {
  url: string;
  params?: any;
  func?: Func;
  args: any[];
}

export interface Summary {
  header: string;
  subHeader?: string;
  lines?: string[];
  codes?: string[];
  summaryHeader?: string;
  summarys?: Summary[];
  templates?: TemplateRef<ElementRef>[];
}

export interface IKeyValue<TKey, TValue> {
  id: TKey;
  value: TValue;
}
