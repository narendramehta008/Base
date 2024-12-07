import { Component, OnInit } from '@angular/core';
import { UtilsService } from '@app/shared/services/utils.service';
import { environment } from '@environments/environment.development';
import { SwaggerRequest, SwaggerResponse, SwaggerUIBundle, SwaggerUIStandalonePreset } from 'swagger-ui-dist';
@Component({
  selector: 'app-tools',
  templateUrl: './tools.component.html',
  styleUrl: './tools.component.scss'
})
export class ToolsComponent implements OnInit {

  constructor(private utils:UtilsService){
    this.loadUrl(environment.serverOrigin+environment.swaggerUrl);
  }

  data:any={};

  ngOnInit(): void {
     let preset: any = SwaggerUIBundle;
   let ui= SwaggerUIBundle({
      dom_id: '#swagger-ui',
      layout: 'StandaloneLayout',
      presets: [
        preset.presets?.apis,
        SwaggerUIStandalonePreset
      ],
      url: 'https://petstore.swagger.io/v2/swagger.json',
      docExpansion: 'none',
      operationsSorter: 'alpha',
      responseInterceptor: (response: SwaggerResponse) => {
        return response;
      },
      requestInterceptor:(request:SwaggerRequest)=>{
        return request;
      }
    });

    
  }

  private loadUrl(url:string){

    this.utils
    .postRequest(environment.apiEndPoint.apiManager.get, {
      url: url,
      headers: [
        {
          key: 'User-Agent',
          value:
            'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36',
        },
      ],
    })
    .subscribe((res) => {
      this.data=res;
    });
  }
  
  wait(ms:number){
    setTimeout(() => {
      new Promise(()=>{});
     }, ms);
  }
}
