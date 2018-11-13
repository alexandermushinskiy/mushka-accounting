import { ViewParameter } from './view-parameter.model';
import { ViewParameterName } from './view-parameter-name.enum';

export class View {
  constructor(public name: string,
              public maxResultsNumber: number,
              public parameters: ViewParameter[],
              public id: string = null,
              public username: string = null,
              public isShared: boolean = false) {
  }

  hasWorkgroups() {
    return this.parameters.some(parameter => parameter.name === ViewParameterName.WORKGROUP);
  }
}
