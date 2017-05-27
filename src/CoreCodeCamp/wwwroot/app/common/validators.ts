module CodeCamp.Common {

  declare var Vue: any;
  declare var VeeValidate: any;

  export function createValidators() {

    let passwordValidation = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,25}$/;
    VeeValidate.Validator.extend('strongPassword', {
      getMessage: field => 'The ' + field + ' requires an uppercase, a lower case and a number.',
      validate: value => passwordValidation.test(value)
    });

  }
}