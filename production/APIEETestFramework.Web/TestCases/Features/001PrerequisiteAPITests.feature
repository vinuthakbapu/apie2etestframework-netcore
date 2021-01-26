@Prerequisite @APITest
Feature: 001_Regression_API_Prerequisite_Tests
E2ETests001	Create Customer
E2ETests002	Load SOM
E2ETests003	Check SOM
E2ETests026 Delete Customer

@CustomerTest
Scenario: E2ETests001: Create Customer
    When Create Customer 
	Then Verify the 200 status code for Create Customer
	And Store CustomerId

Scenario: E2ETests002:Check SOM for created status
    When Check SOM
	Then Verify the 200 status code validate for CREATED status

Scenario: E2ETests003:Load SOM
    When Load SOM
	Then Verify the 200 status code for Load SOM

Scenario: E2ETests004A:Check SOM
    When Check SOM
	Then Verify the 200 status code validate for COMPLETE status