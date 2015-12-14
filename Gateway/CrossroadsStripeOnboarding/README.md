#CrossroadsStripeOnboarding

##Overview
This is a console application which will create Stripe Plans and Subscriptions for MinistryPlatform recurring gifts, associating these to the proper Stripe Customer.
This is intended to be used to complete the migration of recurring gifts from Legacy crossroads.net (and Forte) to MinistryPlatform (and Stripe), and will also verify
the migration after it has been completed.

##Prerequisites & Assumptions
The following criteria are required for this process to successfully run.
1. Stripe has processed the Forte export file and has created Customers in the Live environment
2. Stripe has delivered their JSON Mapping file to Crossroads
** This file contains a mapping from "old" customer id (person ID in Legacy) to a "new" Customer ID in Stripe
** The new Customer in Stripe also has the proper payment (bank or credit card) information associated to the Customer as the Default Source
3. The data migration has been run, to create MinistryPlatform Donors, Recurring Gifts, and Donor Accounts from Legacy crossroads.net
4. The following environment variables are set to proper values on the Windows PC/VM where the executable will run:
** APP_LOG_ROOT - a path where Log4Net log files will be written (e.g. c:\logfiles)
** CRDS_API_ENDPOINT - the Gateway API endpoint (e.g. https://gatewayint.crossroads.net/gateway/)
** CRDS_CMS_ENDPOINT - the CMS API endpoint (e.g. https://contentint.crossroads.net/)
** MP_API_DB_USER - The SQLServer userid for logging in to the MP database (e.g. ApiUser)
** MP_API_DB_PASSWORD - The SQLServer password for the DB user
** STRIPE_AUTH_TOKEN - The private/secret Stripe authorization token

##Usage
To execute the migration, simply run the console application with no arguments, and it will ask for an input file - this should be the full path to the Stripe JSON Mapping file.
To run in "verification" mode after the migration is complete, run the executable again in verification mode with a "-V" command-line argument.  This will output a CSV file
with the following fields:
* mpRecurringGiftId - The recurring gift ID from MinistryPlatform
* stripeSubscriptionId - The Stripe subscription ID
* mpAmount - The amount of the gift in MinistryPlatform
* stripeAmount - The amount of the gift in Stripe
* mpFrequency - The frequency of the gift in MP (Weekly or Monthly)
* stripeFrequency - The frequency of the gift in Stripe (Weekly or Monthly)
* mpRepeat - The recurrence of the gift in MP (day of month, or day of week)
* stripeRepeat - The recurrence of the gift in Stripe (day of month, or day of week)
* mpDonorAccountType - The account type used for the gift in MP (Bank or CreditCard)
* stripeAccountType - The account type used for the gift in Stripe (Bank or CreditCard)
* mpAccountLast4 - The last 4 digits of the account number in MP
* stripeAccountLast4 - The last 4 digits of the account number in Stripe
* success - A boolean indicating "True" if the fields match between MP and Stripe, "False" if not
* errorMessage - A detailed message indicating the reason for mismatch, if "success" was False

There will be a row for every recurring gift in MP, regardless of whether the gift was migrated or newly created by a Community Member.  Any rows with success=False should be investigated
to fix the mismatch.