<template>
  <div v-if="talk">
    <span>{{ talk.title }}</span>
    <button class="btn btn-sm btn-success pull-right"><i class="fa fa-question"></i></button>
  </div>
</template>

<script>
export default {
  props: ["talk"]
}
</script>

<style>

</style>
