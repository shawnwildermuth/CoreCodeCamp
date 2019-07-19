<template>
  <span @click="onClick()" class="clickable">{{ label }} <i class="fa" v-show="sortInfo.column == columnName" :class="sortInfo.ascending ? 'fa-caret-down' : 'fa-caret-up'"></i></span>
</template>

<script>
export default {
  props: ["label", "sortInfo", "columnName", "sortAction"],
  methods: {
    onClick() {
      this.$store.dispatch(this.sortAction, this.columnName);
    }
  }
}
</script>

<style>
.clickable {
  cursor: pointer;
  -webkit-touch-callout: none; /* iOS Safari */
    -webkit-user-select: none; /* Safari */
     -khtml-user-select: none; /* Konqueror HTML */
       -moz-user-select: none; /* Firefox */
        -ms-user-select: none; /* Internet Explorer/Edge */
            user-select: none; /* Non-prefixed version, currently
                                  supported by Chrome and Opera */
}
</style>
