# Test Plan for the Orders Service

We consider the Orders service to constitute a bounded context (BC), i.e. it makesense to test the service itself in isolation.

[TOC]

## Input and Stakeholders

Available documentation which this test plan is based upon,

- Requirements document
- Solution Design document
- Endpoint documentation

Stakeholders,

- ...

## Dimensions

The possible dimensions identified are the following,

| Input parameter | Possible values |
| --------------- | --------------- |
| Asset type | stock, CFD |
| Order type | limit, trailing step |
| Buy/sell type | buy, sell |
| Order duration | (a date with time) |
| Id | (an integer) |
| Price | (a decimal number) |
| Amount | (a decimal number) |
| Position ID | (an integer) |
| Distance | (a decimal number) |
| Step | (a decimal number) |

| IC item | Possible values |
| --------------- | --------------- |
| Asset |  |
| Logged-in user |  |

We have not done any equivalence class partitioning or boundary analysis of the possible values in the tables above.

## Areas

Based on requirements, it seems that interactions vary significantly for different order types. We therefore decide in the following to *divide into areas by order type*, so that we have one area for `limit entry orders` and another area for `trailing step related orders`.

## Test Analysis

Requirements indicate interaction among several dimensions, as e.g. it may be allowed to sell an `asset` that the `logged-in user` does not own, but this depends on at least the `logged-in user`, the `asset type` and a concrete `asset` IC item. However, by closer inspection of the requirements, architectural documentation, and actual code paths, it can be concluded that this functionality is entirely implemented outside the BC and is thus not relevant to cover. It can also be concluded that the two `asset types` can be collapsed into a single equivalence class.

Armed with this knowledge, we can conclude that some of the dimensions don't contribute to the combinatorial explosion for a given area (`order type`), as we can use any `asset type` and we can assume that the user is authenticated and that authorization to trade the `asset` is handled outside the BC. Therefore, we will omit these dimensions from further analysis.

The dimensions left to consider are,

| Input parameter | Possible values |
| --------------- | --------------- |
| Order type | limit, trailing step |
| Buy/sell type | buy, sell |
| Order duration | (a date with time) |
| Id | (an integer) |
| Price | (a decimal number) |
| Amount | (a decimal number) |
| Position ID | (an integer) |
| Distance | (a decimal number) |
| Step | (a decimal number) |

| IC item | Possible values |
| --------------- | --------------- |
| Asset |  |

### Test Scenarios

Within the BC, the responsibilities of the Order service are (1) advanced input parameter validation, and (2) mapping combinations of input parameters to the API of the dependent TBL service (see Fig. 3), which will then do the actual order placement.

| Scenario                   | Category                 | Comments | Scenario ID     |
| -------------------------- | ------------------------ | -------- | --------------- |
| Simple input parameter validation | InputValidation |          | InputValidation |
| Advanced input parameter validation | CoreFunctionality |          | AdvancedInputValidation |
| Mapping combinations of input parameters | CoreFunctionality        |          | Mapping |

### Target Area: Trailing Stop Order Type

First, we consider 1-way interactions. This is typically simple input validations. In our example, this is relevant for the `id`, `price`, `amount`, and `order duration` parameters. Covering a single invalid value for each is sufficient, so 4 tests will cover these. In the following analysis, we can use any valid value for these dimensions.

| Id       | Price    | Amount   | Duration | Test case |
| -------- | -------- | -------- | -------- | -------- |
| ~Invalid | Valid    | Valid    | Valid    | PostOrderMustReportErrorWhenInvalidId |
| Valid    | ~Invalid | Valid    | Valid    | PostOrderMustReportErrorWhenInvalidPrice |
| Valid    | Valid    | ~Invalid | Valid    | PostOrderMustReportErrorWhenInvalidAmount |
| Valid    | Valid    | Valid    | ~Invalid | PostOrderMustReportErrorWhenInvalidDuration |

In the following analysis, we can use any valid value for these dimensions. This leaves us the following dimensions to consider,

| Input parameter | Possible values |
| --------------- | --------------- |
| Buy/sell type | buy, sell |
| Position ID | valid, invalid, not there |
| Distance | valid, invalid, not there |
| Step | valid, invalid, not there |

| IC item | Possible values |
| --------------- | --------------- |
| Asset | tradable, not tradable |

We are down to only 108 combinations, which is manageable, especially if we assume only 2-way interaction, in which case we can reduce the number to 11. But we want to deduce actual interaction, so we continue one step further based on our knowledge about Trading service responsibilities

> Use the file trailing-stop.txt with the Microsoft PICT tool in order to find the combinations.
> Find pairwise (2-way) combinations like this,
> pict .\trailing-stop.txt  > x.txt
> Find all (5-way) combinations like this,
> pict .\trailing-stop.txt /o:5 > x.txt

The parameters `position ID`, `distance` and `step` are handled by advanced input parameter validation. However, looking at requirements and code paths, it is reasonable to assume that validation functionality of the three parameters do not interact. So again, we need 3 tests for invalid values and 3 tests for the values not provided.

| Position ID | Distance | Step  | Test case |
| -------- | -------- | -------- | -------- | -------- |
| ~Invalid | Valid    | Valid    | PostOrderMustReportErrorOnInvalidPositionId |
| Valid | ~Invalid    | Valid    | PostOrderMustReportErrorOnInvalidDistance |
| Valid | Valid    | ~Invalid    | PostOrderMustReportErrorOnInvalidStep |
| ~Not there | Valid    | Valid    | PostOrderMustReportErrorOnPositionIdNotThere |
| Valid | ~Not there    | Valid    | PostOrderMustReportErrorOnDistanceNotThere |
| Valid | Valid    | ~Not there    | PostOrderMustReportErrorOnStepNotThere |

After this, we can assume that the three parameters above do not contribute to the combinatorial explosion.

The only parameters we still need to consider are the `buy/sell type` and the `asset` being tradable or not, in total 4 tests,

| Buy/sell type | Asset | Test case |
| -------- | -------- | -------- | -------- | -------- |
| Buy | Tradable   | PostOrderMustBuyWhenAssetIsTradable |
| Sell | Tradable   | PostOrderMustSellWhenAssetIsTradable |
| Buy | Not tradable   | PostOrderMustNotBuyWhenAssetIsNotTradable |
| Sell | Not tradable   | PostOrderMustNotSellWhenAssetIsNotTradable |

### Target Area: Limit Order Type

### Out of scope

Nothing has been explicitly left out, so far.
