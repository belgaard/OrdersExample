# Test Plan for the Orders Service

[TOC]

## Input and Stakeholders

Here you will add available documentation which this test plan is based upon,
- Requirements document
- Solution Design document
- Endpoint documentation

Stakeholders,
 - ...

## Test Analysis

Here you will analyze and document the testing needed in order to get sufficient confidence.

This is a combinatorial analysis based on dimensions (input and existing state) and relevant values.
See more details here: https://wiki/display/OpenAPI/Test+Analysis

### Target Areas and Dimensions

Here you will identify general dimensions for the entire service. This is sometimes useful, but can be omitted if it does not seem to provide value.

For the example service we have identified the following dimensions,

- Authorization
- End-point
- ...

### Test Scenarios

Here you will identify "scenarios", which are essentially headlines, or groupings, of individual test cases. See more details here: https://wiki/pages/viewpage.action?pageId=92233716#TestPlanGuidelines(Feature&ServiceGroup)-TestScenarios. For consistency across all services, you can use the categories from here https://wiki/display/OpenAPI/Test+Glossary

The following test scenarios were derived for the example service,

| Scenario                   | Category                 | Comments | Scenario ID     |
| -------------------------- | ------------------------ | -------- | --------------- |
| Access to anonymous end-points, restricted access to end-points which require authorization. | Security |          | Authorization |
| Core functionality | CoreFunctionality        |          | BasicFunctional |

### *Autorization controller* aka "auth"

Dimensions,
- Authorization: Authorized, NotAutorized
- End-point: anon, requires-auth

Since we have two dimensions, each with two values, we have four combinations in total.

We have decided that *Authorized/anon* can be omitted without significant loss of confidence, so we leave the Test case column blank for this combination.

| Authorization | End-point  | Test case |Comments |
| ------------------------- | ----------------------------- | --------- |--------- |
| NotAutorized | anon         | AnonMustReturnOkWhenNotAuthenticated ||
| Authorized | anon |  |Not worth testing|
| NotAutorized | requires-auth | RequiresAuthMustFailWhenNotAuthenticated ||
| Authorized | requires-auth | RequiresAuthMustNotFailWhenAuthenticated ||

### Out of scope

Here you will mention anything which has been explicitly left out.

