<template>
  <div class="pull-right">
    <button class="btn btn-sm btn-secondary" @click="addRoom">New Room</button>
    <button class="btn btn-sm btn-secondary" @click="addTimeslot">New Timeslot</button>
  </div>
</template>

<script>
import { mapActions } from "vuex";

export default {
  methods: {
    ...mapActions(["addRoom", "addTimeslot"])
  }
}
</script>

<style>

</style>
