import { AfterViewInit, Component, EventEmitter, Input, Output } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import * as ldrs from 'ldrs';

@Component({
  selector: 'btsp-loader',
  templateUrl: './loader.component.html',
  styleUrl: './loader.component.scss'
})
export class LoaderComponent implements AfterViewInit {

  loaders: any = ldrs;
  loadersList = ['chaoticOrbit', 'dotPulse', 'dotSpinner', 'dotStream', 'dotWave', 'grid', 'hatch', 'helix', 'hourglass'
    , 'infinity', 'jelly', 'jellyTriangle', 'leapfrog', 'lineSpinner', 'lineWobble', 'metronome', 'mirage'
    , 'miyagi', 'momentum', 'newtonsCradle', 'orbit', 'ping', 'pinwheel', 'pulsar', 'quantum', 'reuleaux',
    'ring', 'ring2', 'ripples', 'spiral', 'square', 'squircle', 'superballs', 'tailChase', 'tailspin', 'treadmill'
    , 'trefoil', 'trio', 'waveform', 'wobble', 'zoomies'];
  @Input() loader: string = '';
  ringCollectionHtml: any='';
  @Input() size: string = '';
  @Input() color: string = '';
  @Input() speed: string = '';
  @Output() connectedCallback: EventEmitter<any> = new EventEmitter();
  @Output() attributeChangedCallback: EventEmitter<any> = new EventEmitter();

  constructor(private sanitizer: DomSanitizer) {
    
  }
  ngAfterViewInit(): void {
    if (this.loader)
      this.loaders?.[this.loader].register('btsp-loader');
    else
      this.addRings();
  }


  addRings() {
    this.loadersList.forEach((tag) => {
    
      let tagname = `l-${tag}`.toLocaleLowerCase();
      this.ringCollectionHtml += `<div class='col'>
        <${tagname}  color="${this.randomColor()}" size="100"></${tagname}>
        ${tag}
       </div>`;
      this.loaders?.[tag]?.register(tagname);

    });
    this.ringCollectionHtml = this.sanitizer.bypassSecurityTrustHtml(this.ringCollectionHtml);
  }

  randomColor() {
    return '#' + (Math.random().toString(16) + "000000").substring(2, 8);
  }
}
