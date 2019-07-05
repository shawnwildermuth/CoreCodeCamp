<template>
  <div role="tabpanel" class="tab-pane fade" id="slots">
    <table class="table table-condensed table-responsive table-striped">
      <thead>
        <tr>
          <td class="col-md-10">Time</td>
          <td class="col-md-2"></td>
        </tr>
      </thead>
      <tr v-for="timeSlot in timeSlots">
        <td>{{ timeSlot.time | formatTime }}</td>
        <td>
          <button class="btn btn-sm btn-danger" title="Delete" @@click="onDeleteTimeSlot(timeSlot)">
            <i class="fa fa-remove"></i>
          </button>
        </td>
      </tr>
    </table>
    <input v-model="newTimeSlot" name="newTimeSlot" />
    <button class="btn btn-success" @@click="onSaveTimeSlot()">
      <i class="fa fa-plus"></i>
    </button>
  </div>
</template>

<script>
export default {};
</script>

<style>
</style>
