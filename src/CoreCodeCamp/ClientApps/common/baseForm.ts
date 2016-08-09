export abstract class BaseForm {

  isBusy: boolean = false;
  error: string = null;

  showError(err) {
    this.error = err;
    this.isBusy = false;
  }

}