module.exports = {
  MyTrips:
    [
      {
        eventParticipantId:2631206,
        tripStartDate:'Jul 25, 2015',
        tripEnd:'Dec 31, 2015',
        tripName:'(d) NKY Big Trip 2015',
        fundraisingDays:136,
        fundraisingGoal:1000,
        totalRaised:500,
        tripGifts:[
          {
            donorId: 214,
            donorNickname:'TJ',
            donorLastName:'Maddox',
            donorEmail:'tmaddox33+mp1@gmail.com',
            donationAmount:500,
            donationDate:'8/13/2015',
            registeredDonor:true,
            donationDistributionId: 149,
            paymentTypeId: 4,
          }]
      },
      {
        eventParticipantId:0,
        tripStartDate:'Jul 27, 2015',
        tripEnd:'Jul 30, 2015',
        tripName:'(d) Sweden Trip 2015',
        fundraisingDays:0,
        fundraisingGoal:2000,
        totalRaised:2000,
        tripGifts:[
          {
            donorId: 123,
            donorNickname:'Anonymous',
            donorLastName:'',
            donorEmail:'andrew.canterbury@ingagepartners.com',
            donationAmount:1900,
            donationDate:'8/11/2015',
            registeredDonor:true,
            anonymous: true,
            donationDistributionId: 179,
            paymentTypeId: 4,
          },
          {
            donorId: 123,
            donorNickname:'Scholorship',
            donorLastName:'',
            donorEmail:'tmaddox33+mp1@gmail.com',
            donationAmount:100,
            donationDate:'8/8/2015',
            registeredDonor: false,
            donationDistributionId: 169,
            paymentTypeId: 9,
          },
          {
            donorId: 1333,
            donorNickname:'Transfer',
            donorLastName:'',
            donorEmail:'andrew.canterbury@ingagepartners.com',
            donationAmount:1900,
            donationDate:'8/11/2015',
            registeredDonor:true,
            anonymous: false,
            donationDistributionId: 234,
            paymentTypeId: 13,
          },
        ]
      }
    ],
  TripParticipant: {
    email: 'matt.silbernagel@ingagepartners.com',
    participantId: 2213526,
    participantName: 'Matt Silbernagel',
    showGiveButton: true,
    showShareButtons: false,
    participantPhotoUrl: 'http://crossroads-media.imgix.net/images/avatar.svg',
    trips: [{
      tripEnd: 'Dec 28, 2015',
      tripParticipantId: 4593680,
      tripStartDate: 1450656000,
      tripStart: 'Dec 21, 2015',
      tripName: '2015 December GO South Africa',
      programId: 2213526,
      programName: '2015 December GO South Africa',
      campaignId: 123456789,
      campaignName: 'test campaign',
      pledgeDonorId: 23232323
    }]
  },
  Trip: {
    tripEnd: 'Dec 28, 2015',
    tripParticipantId: 4593680,
    tripStartDate: 1450656000,
    tripStart: 'Dec 21, 2015',
    tripName: '2015 December GO South Africa',
    programId: 2213526,
    programName: '2015 December GO South Africa',
    campaignId: 123456789,
    campaignName: 'test campaign',
    pledgeDonorId: 23232323
  }
};
