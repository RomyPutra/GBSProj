import { PlexformTemplatePage } from './app.po';

describe('Plexform App', function() {
  let page: PlexformTemplatePage;

  beforeEach(() => {
    page = new PlexformTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
