Feature: CustomerFeature

@customer
Scenario: Get Customer By Id
	Given a customer exists
	When the customer is asked for
	Then the result is Ok
	And the result contains the customer