<template>
  <div class="row text-left">
    <div class="col-md-10 col-md-offset-1">
      <camp></camp>
      <div>
        <router-link class="btn btn-info" to="/camp/edit/edit" :camp="this.$store.state.currentCamp"><i class="fa fa-pencil"></i> Edit Camp</router-link>&nbsp;
        <router-link class="btn btn-info" to="/camp/edit/new" :camp="this.$store.state.currentCamp"><i class="fa fa-gift"></i> New Camp</router-link>
      </div>
      
    </div>
  </div>
</template>

<script>
import Camp from "./camp.vue";

export default {
  components: { Camp }
};
</script>