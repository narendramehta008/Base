import { Component, Input, OnInit } from '@angular/core';
import { Summary } from '@app/core/models/Funcs';

@Component({
  selector: 'btsp-summary',
  templateUrl: './summary.component.html',
  styleUrl: './summary.component.scss',
})
export class SummaryComponent implements OnInit {
  @Input() summarys: Summary[] = [];
  @Input() showExpander = false;
  @Input() detailClass = '';
  expanded = false;

  constructor() {}

  ngOnInit(): void {}

  expandAll() {
    this.expanded = !this.expanded;
    Array.from(document.getElementsByClassName('bg-bright')).forEach(
      (element) => {
        this.expanded && element.setAttribute('open', '');
        this.expanded || element.removeAttribute('open');
      }
    );
  }
}
