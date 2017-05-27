namespace CodeCamp.Common {

  export let helpers = {
    isPristine(fields) {
      return Object.keys(fields).every(field => {
        return fields[field] && fields[field].pristine;
      });
    }
  }
}