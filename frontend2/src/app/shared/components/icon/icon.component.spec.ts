import { Shallow } from 'shallow-render';

import { IconComponent } from './icon.component';
import { IconModule } from './icon.module';

describe('IconComponent', () => {
  let shallow: Shallow<IconComponent>;

  beforeEach(() => {
    shallow = new Shallow(IconComponent, IconModule);
  });

  it('should create', async () => {
    const { instance } = await shallow.render();

    expect(instance).toBeTruthy();
  });
});
