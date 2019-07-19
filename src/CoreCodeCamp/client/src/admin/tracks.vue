<template>
  <div role="tabpanel" class="tab-pane fade" id="tracks">
    <table class="table table-condensed table-responsive table-striped">
      <thead>
        <tr>
          <td class="col-md-10">Name</td>
          <td class="col-md-2"></td>
        </tr>
      </thead>
      <!-- <tr v-for="track in tracks">
                  <td>{{ track.name }}</td>
                  <td><button class="btn btn-sm btn-danger" title="Delete" @@click="onDeleteTrack(track)"><i class="fa fa-remove"></i></button></td>
      </tr>-->
    </table>
    <input v-model="newTrack" name="newTrack" />
    <button class="btn btn-success" @@click="onSaveTrack()">
      <i class="fa fa-plus"></i>
    </button>
  </div>
</template>

<script>
export default {};
</script>

<style>
</style>
