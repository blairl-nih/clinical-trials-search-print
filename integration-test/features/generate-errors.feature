Feature: The page generation handler responds approriately to invalid inputs.

Background:
    * url apiHost

Scenario: Status 400 on non-JSON body.

    Given path 'CTS.Print', 'GenCache'
    And request 'foo = bar'
    When method post
    Then status 400
    And match response == 'Unable to parse request body.'

Scenario: Status 400 on broken JSON body.

    Given path 'CTS.Print', 'GenCache'
    And request '{"trial_ids": [ "NCI-2015-01906" '
    When method post
    Then status 400
    And match response == 'Unable to parse request body.'

Scenario: Status 400 when trial_ids is missing from the request body.

    Given path 'CTS.Print', 'GenCache'
    And request
    """
    {
        "link_template": "/about-cancer/treatment/clinical-trials/search/v?t=C3208&st=C3471&a=24&loc=1&z=20850&zp=100&rl=2&id=<TRIAL_ID>",
        "search_criteria": null
    }
    """
    When method post
    Then status 400
    And match response == "Field 'trial_ids' not found."

Scenario Outline: Status 400 when trial_ids is present but null or empty.

    Given path 'CTS.Print', 'GenCache'
    And request
    """
    {
        "trial_ids": trial_list
        "link_template": "/about-cancer/treatment/clinical-trials/search/v?t=C3208&st=C3471&a=24&loc=1&z=20850&zp=100&rl=2&id=<TRIAL_ID>",
        "search_criteria": null
    }
    """
    When method post
    Then status 400
    And match response == "Field 'trial_ids' not found."

    Examples:
    | trial_list |
    | []         |
    | null       |

Scenario Outline: Status 400 when the new_search_link field contains invalid characters (e.g. an injection attack)

    Given path 'CTS.Print', 'GenCache'
    And request
    """
    {
        "trial_ids": [ "NCI-2018-03694", "NCI-2020-00544", "NCI-2018-01575" ],
        "link_template": "/about-cancer/treatment/clinical-trials/search/v?t=C3208&st=C3471&a=24&loc=1&z=20850&zp=100&rl=2&id=<TRIAL_ID>",
        "search_criteria": null,
        "new_search_link": search_link
    }
    """
    When method post
    Then status 400
    And match response == "Field 'new_search_link' has an invalid value."

    Examples:
    | search_link |
    | "/about-cancer/treatment/clinical-trials/search/advanced\\\" onClick=\\\"alert('Oops!')\\\"" |
    | "/about-cancer/treatment/clinical-trials/search/advanced\\\" ></a> <p>evil markdown</p>"     |


Scenario Outline: Status 400 when the link_template field contains invalid characters (e.g. an injection attack)

    Given path 'CTS.Print', 'GenCache'
    And request
    """
    {
        "trial_ids": [ "NCI-2018-03694", "NCI-2020-00544", "NCI-2018-01575" ],
        "new_search_link": "/about-cancer/treatment/clinical-trials/search/advanced",
        "search_criteria": null,
        "link_template": template
    }
    """
    When method post
    Then status 400
    And match response == "Field 'link_template' has an invalid value."

    Examples:
    | template |
    | "/about-cancer/treatment/clinical-trials/search/v?t=C3208&st=C3471&a=24&loc=1&z=20850&zp=100&rl=2&id=<TRIAL_ID>\\\" onClick=\\\"alert('Oops!')\\\"" |
    | "/about-cancer/treatment/clinical-trials/search/v?t=C3208&st=C3471&a=24&loc=1&z=20850&zp=100&rl=2&id=<TRIAL_ID>\\\" ></a> <p>evil markdown</p>"     |


