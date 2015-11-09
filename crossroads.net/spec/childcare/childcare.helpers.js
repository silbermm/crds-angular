(function() {

  module.exports = {
    children: [
      {
        id: 98765,
        firstName: 'baby',
        lastName: 'Silbernagel',
        age: 1,
        grade: 0
      },
      {
        id: 9234265,
        firstName: 'Miles',
        lastName: 'Silbernagel',
        age: 7,
        grade: 2
      }
    ],

    event: {
      eventId: 1234,
      eventTitle: 'Test title'
    },

    childcareEvent: {
      eventId: 123,
      eventTitle: 'Test Childcare Title'
    },

    childcareEvents: [
      {eventId: 123, eventTitle: 'Test Childcare Title' },
      {eventId: 321, eventTitle: 'Another Childcare Test' }
    ]
  };

})();
